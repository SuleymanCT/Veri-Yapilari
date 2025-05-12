// === scripts.js ===
const API_URL = "https://localhost:7183/api";

let nodePositions = {};

async function addCustomer() {
    const name = document.getElementById("nameInput").value.trim();
    if (!name) return;

    await fetch(`${API_URL}/customer/add`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(name)
    });

    document.getElementById("nameInput").value = "";
    await updateUI();
}

function renderQueue(queue) {
    const container = document.getElementById("queueContainer");
    const existing = Array.from(container.children).map(c => c.textContent);

    if (JSON.stringify(existing) === JSON.stringify(queue)) return;

    container.innerHTML = "";
    queue.forEach(name => {
        const div = document.createElement("div");
        div.className = "queue-item";
        div.textContent = name;
        container.appendChild(div);
    });
}

function renderReps(reps) {
    const container = document.getElementById("repContainer");

    container.innerHTML = "";
    reps.forEach(rep => {
        const div = document.createElement("div");
        div.className = `rep-box ${rep.isBusy ? "busy" : "available"}`;

        if (rep.isBusy && rep.currentCustomer) {
            div.textContent = `${rep.name} - ${rep.currentCustomer} ile görüşüyor`;
        } else {
            div.textContent = `${rep.name} - Boşta`;
        }

        container.appendChild(div);
    });
}

async function updateUI() {
    try {
        const [queueRes, repsRes] = await Promise.all([
            fetch(`${API_URL}/customer/queue`),
            fetch(`${API_URL}/customer/status`)
        ]);

        const queue = await queueRes.json();
        const reps = await repsRes.json();

        renderQueue(queue);
        renderReps(reps);
    } catch (err) {
        console.error("Veri çekme hatası:", err);
    }
}

async function fetchBSTs() {
    const res = await fetch(`${API_URL}/customer/trees`);
    return await res.json();
}

function drawBST(root, svg, x, y, spacing = 60, depth = 0, parent = null) {
    if (!root) return;

    const nodeX = x;
    const nodeY = y + depth * 100;

    if (parent) {
        const line = document.createElementNS("http://www.w3.org/2000/svg", "line");
        line.setAttribute("x1", parent.x);
        line.setAttribute("y1", parent.y + 20);
        line.setAttribute("x2", nodeX);
        line.setAttribute("y2", nodeY - 20);
        line.setAttribute("stroke", "black");
        svg.appendChild(line);
    }

    const circle = document.createElementNS("http://www.w3.org/2000/svg", "circle");
    circle.setAttribute("cx", nodeX);
    circle.setAttribute("cy", nodeY);
    circle.setAttribute("r", 20);
    circle.setAttribute("fill", "#3498db");
    svg.appendChild(circle);

    const text = document.createElementNS("http://www.w3.org/2000/svg", "text");
    text.setAttribute("x", nodeX);
    text.setAttribute("y", nodeY + 5);
    text.setAttribute("text-anchor", "middle");
    text.setAttribute("fill", "#fff");
    text.setAttribute("font-size", "14");
    text.textContent = root.id;
    svg.appendChild(text);

    if (root.left) {
        drawBST(root.left, svg, x - spacing / Math.pow(2, depth), y, spacing, depth + 1, { x: nodeX, y: nodeY });
    }

    if (root.right) {
        drawBST(root.right, svg, x + spacing / Math.pow(2, depth), y, spacing, depth + 1, { x: nodeX, y: nodeY });
    }
}

async function renderBSTCanvas() {
    const svg = document.getElementById("bstCanvas");
    svg.innerHTML = "";

    const trees = await fetchBSTs();
    let x = 150;

    for (const [repName, tree] of Object.entries(trees)) {
        if (!tree) continue;

        const label = document.createElementNS("http://www.w3.org/2000/svg", "text");
        label.setAttribute("x", x);
        label.setAttribute("y", 20);
        label.textContent = `Temsilci ${repName}`;
        label.setAttribute("font-size", "16");
        label.setAttribute("fill", "black");
        svg.appendChild(label);

        drawBST(tree, svg, x, 50);
        x += 350;
    }
}

async function fetchGraphStructure() {
    const res = await fetch(`${API_URL}/graph/structure`);
    return await res.json();
}

function drawGraph(graph) {
    const svg = document.getElementById("graphCanvas");
    svg.innerHTML = "";

    nodePositions = {};
    const reps = Object.keys(graph);
    const centerY = 200;
    const spacingX = 250;
    let startX = 150;

    reps.forEach((rep, index) => {
        const x = startX + index * spacingX;
        const y = centerY;
        nodePositions[rep] = { x, y };

        const rect = document.createElementNS("http://www.w3.org/2000/svg", "rect");
        rect.setAttribute("x", x - 40);
        rect.setAttribute("y", y - 25);
        rect.setAttribute("width", 80);
        rect.setAttribute("height", 50);
        rect.setAttribute("fill", "#2ecc71");
        rect.setAttribute("rx", "10");
        svg.appendChild(rect);

        const label = document.createElementNS("http://www.w3.org/2000/svg", "text");
        label.setAttribute("x", x);
        label.setAttribute("y", y + 5);
        label.setAttribute("text-anchor", "middle");
        label.setAttribute("fill", "white");
        label.setAttribute("font-size", "16");
        label.textContent = rep;
        svg.appendChild(label);
    });

    for (const [from, tos] of Object.entries(graph)) {
        const fromPos = nodePositions[from];
        tos.forEach(to => {
            const toPos = nodePositions[to];
            const line = document.createElementNS("http://www.w3.org/2000/svg", "line");
            line.setAttribute("x1", fromPos.x);
            line.setAttribute("y1", fromPos.y + 25);
            line.setAttribute("x2", toPos.x);
            line.setAttribute("y2", toPos.y - 25);
            line.setAttribute("stroke", "black");
            line.setAttribute("marker-end", "url(#arrow)");
            svg.appendChild(line);
        });
    }

    const defs = document.createElementNS("http://www.w3.org/2000/svg", "defs");
    defs.innerHTML = `
    <marker id="arrow" markerWidth="10" markerHeight="10" refX="5" refY="5"
        orient="auto-start-reverse" markerUnits="strokeWidth">
      <path d="M 0 0 L 10 5 L 0 10 z" fill="black" />
    </marker>`;
    svg.appendChild(defs);
}

async function fetchTransfers() {
    const res = await fetch(`${API_URL}/graph/recent-transfers`);
    return await res.json();
}

function animateTransfer(fromPos, toPos, svg) {
    const circle = document.createElementNS("http://www.w3.org/2000/svg", "circle");
    circle.setAttribute("r", 6);
    circle.setAttribute("fill", "red");

    svg.appendChild(circle);

    let t = 0;
    const steps = 30;

    const interval = setInterval(() => {
        if (t > 1) {
            svg.removeChild(circle);
            clearInterval(interval);
            return;
        }

        const x = fromPos.x + (toPos.x - fromPos.x) * t;
        const y = fromPos.y + (toPos.y - fromPos.y) * t;

        circle.setAttribute("cx", x);
        circle.setAttribute("cy", y);

        t += 1 / steps;
    }, 20);
}
async function renderGraphList() {
    const container = document.getElementById("graphListContainer");
    container.innerHTML = "";

    const res = await fetch(`${API_URL}/graph/structure`);
    const graph = await res.json();

    const ul = document.createElement("ul");

    for (const [from, toList] of Object.entries(graph)) {
        const li = document.createElement("li");
        li.innerHTML = `<strong>${from}</strong> → ${toList.join(", ")}`;
        ul.appendChild(li);
    }

    container.appendChild(ul);
}

setInterval(() => {
    updateUI();
    renderBSTCanvas();
    renderGraphList(); // SVG yerine bu
    fetchGraphStructure().then(drawGraph);
    fetchTransfers().then(transfers => {
        const svg = document.getElementById("graphCanvas");
        transfers.forEach(t => {
            const fromPos = nodePositions[t.from];
            const toPos = nodePositions[t.to];
            if (fromPos && toPos) {
                animateTransfer(fromPos, toPos, svg);
            }
        });
    });
}, 1000);

updateUI();
renderBSTCanvas();