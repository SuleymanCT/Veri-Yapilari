**Bilet veya Çağrı Merkezi Kuyruğu Simülasyonu**

**Github Proje Adresi**
https://github.com/SuleymanCT/Veri-Yapilari.git 
(final branch: dev8, Geliştirme yapılan branchler: dev5, dev6, dev7, dev8, Yedek branchler: “dev5 backup 3”, “dev-6 backup”, “yedek-dev” )

Proje sürecinde kaynak kod yönetimini GitHub üzerinden gerçekleştirdik. Başlangıçta, yalnızca tek bir branch üzerinden ilerlenmesi planlanmıştı. Ancak ekip üyelerinin aynı anda farklı görevler üzerinde çalışmaları ve zaman zaman aynı dosyalarda çakışmalar yaşanması nedeniyle geliştirme sürecinde birden fazla branch kullanma ihtiyacı doğdu.
Bu kapsamda, dev5, dev6, dev7 ve dev8 isimli geliştirme branch’leri oluşturularak daha verimli, güvenli ve çakışmasız şekilde çalışmalarını sürdürüldü. Zaman zaman branchler üzerinde ekip üyeleri karşılıklı commitler attılar. Geliştirme süreci boyunca bu branch’ler üzerinden düzenli olarak commit işlemleri yapıldı.
Projenin son aşamasında, tüm geliştirilen parçalar bir ekip üyemiz tarafından birleştirildi ve proje final haliyle dev8 branch’ine aktarıldı. Sunum ve teslim işlemleri bu branch üzerinden gerçekleştirildi.
Bu yöntem, hem ekip içi koordinasyonu kolaylaştırdı hem de geliştirme sürecinin daha düzenli ve güvenli ilerlemesini sağladı.

Ayrıca “dev5 backup 3”, “dev-6 backup”, “yedek-dev” adında farklı yedek branchlerde oluşturularak olası problemlerin önüne geçilmesi hedeflendi.
Grup Üyeleri ve Görev Dağılımı

- Süleyman Can Tuğcu (032190012): Planlama, Queue Yapısı, Graph Yapısı, Hash Table Yapısı

- Abdulhakim Tan (032190016): API geliştirme, arayüz entegrasyonu, Hash Table Yapısı

- Sadettin Şahin (032190020): Algoritma analizleri, test ve dokümantasyon, BST Yapısı

**Proje Amacı ve Kapsamı**

Bu projede, çağrı merkezi ya da bilet sırası gibi sistemlerde kullanıcıların sıraya alındığı ve FIFO (First-In First-Out) prensibiyle temsilcilere yönlendirildiği bir simülasyon geliştirilmiştir. Proje kapsamında; kuyruk, Binary Search Tree (BST) ve yönlü Graph yapıları kullanılarak sistemin algoritmik modeli oluşturulmuş, ayrıca HTML/CSS/JavaScript ile desteklenen görsel arayüzde anlık güncellemeler, animasyonlar ve veri görselleştirmeleri sağlanmıştır.

**Kullanılan Teknolojiler**

- C# .NET 6 Web API
- HTML, CSS, JavaScript
- GitHub (versiyon kontrol – Branch: dev8)
- Swagger & Postman (test ve doğrulama)

**Veri Yapıları ve Kullanım Amaçları**

- Queue: Kullanıcılar, FIFO prensibiyle sıraya alınır.
- Binary Search Tree (BST): Temsilcilerin aldığı müşteri ID’leri sıralı tutulur.
- Graph: Temsilciler arası yönlü bağlantılar aktarımı ve yönlendirmeyi sağlar.
- Hash Table: Hızlı ve benzersiz ID üretimi yapılır.
- DTO: BST verileri JSON formatına çevrilerek frontend’e aktarılır.


**Algoritma Analizleri**

Projede kullanılan veri yapılarının performansı ve tercih nedenleri aşağıda analiz edilmiştir:

**🔁 Queue (Kuyruk) vs. Array:**
- Kullanım Yeri: Müşteri sıraya alımı ve çağrılması (CallCenterSimulator.cs)
- Neden Queue Tercih Edildi?
  - Enqueue: O(1), Dequeue: O(1)
  - Array kullanılsaydı Dequeue işleminde elemanlar kaydırılacağı için O(n) zaman maliyeti oluşacaktı.
  - Queue, bellek ve hız açısından çok daha verimli olduğu için tercih edilmiştir.

Nasıl çalışır:
  -FIFO (First-In First-Out) mantığıyla çalışır. İlk gelen müşteri, ilk olarak hizmet alır.

**🌳 Binary Search Tree (BST) vs. Linear List:**
- Kullanım Yeri: Her temsilcinin aldığı müşteri ID’lerini tutmak (CustomerTree.cs)
- Neden BST Tercih Edildi?
  - Insert ve Search: O(log n)
  - Liste kullanılsaydı, arama O(n) olurdu.
  - Ayrıca BST, SVG olarak çizilip kullanıcıya sunulabilir hale getirildi (TreeNodeDto).

Nasıl çalışır:
  -Her düğüm, solundaki düğümden küçük, sağındaki düğümden büyük olacak şekilde yerleştirilir. In-order (sıralı) gezildiğinde ID’ler küçükten büyüğe doğru listelenir.

**🕸️ Graph (Directed) vs. Tablo Temelli Aktarım:**
- Kullanım Yeri: Temsilciler arası müşteri aktarımı modelleme (GraphSimulator.cs)
- Neden Graph Tercih Edildi?
  - Dinamik yönlü bağlantılar sağlandı.
  - Dictionary ile erişim O(1) hızında gerçekleştirildi.
  - Yük paylaşımı ve temsilci yoğunluğu görselleştirilebildi.

Nasıl çalışır:
  -Yönlü graf yapısıdır. Her temsilcinin bağlı olduğu diğer temsilciler listelenir. Temsilci meşgulken, bağlı komşular kontrol edilerek yönlendirme yapılır.

**🧮 Hashing (GetHashCode) ile ID Üretimi:**
- Kullanım Yeri: Müşteri ID üretimi ve BST’ye eklenmesi
- Avantaj:
  - Hızlı ve benzersiz ID üretimi
  - Math.Abs(hash % 10000) ile BST üzerinde dengeli dağılım sağlandı.

Nasıl çalışır:
  -Anahtar-değer eşleşmesi (Key-Value) üzerinden çalışır. O(1) zaman karmaşıklığıyla veri sorgulama ve ekleme yapılır.


**Sonuç:**
Projede kullanılan veri yapıları zaman ve uzay karmaşıklığı açısından verimli olup, gerçek hayattaki çağrı merkezi sistemlerinin algoritmik olarak modellenmesini başarıyla gerçekleştirilmiştir.

**Arayüz:**
Kullanıcı arayüzünde; müşteri ekleme, kuyruk ve temsilci durumu anlık olarak güncellenir. Her temsilcinin hizmet geçmişi BST olarak SVG formatında çizilir, temsilciler arası bağlantılar yönlü graph yapısıyla gösterilir ve müşteri aktarımları animasyonlarla desteklenir.
![image1](https://github.com/user-attachments/assets/3e416750-a1e8-45c9-a880-5cbf056f3759)
![image7](https://github.com/user-attachments/assets/fea430d1-f437-401d-9658-eec72ceb63be)
![image8](https://github.com/user-attachments/assets/f7f39a93-f9e3-4893-8534-095cd5432433)
![image4](https://github.com/user-attachments/assets/e03e0134-41e6-4443-9b90-de7ddd758395)


**Dev7 branch’i, projenin arayüz entegrasyonu yapılmadan önceki son hali olup, terminal üzerinden çalışan ve doğru çıktılar üreten bir sürümüdür. Dev8 branch’inde, özellikle graph kısmında yaşanan küçük arayüz problemleri nedeniyle, Dev7 branch’inde sorunsuz çalışan bölümler referans alınarak gösterilmiştir**

![image2](https://github.com/user-attachments/assets/ba7448c8-b20e-4d9a-8575-fcd757951295)
![image3](https://github.com/user-attachments/assets/f87456a1-89c6-4c2e-8951-7979f27d6f6e)
![image5](https://github.com/user-attachments/assets/ad9aeac2-6fed-41ae-b040-8840fc1296bb)
![image6](https://github.com/user-attachments/assets/62567a44-be53-4f35-8147-c79a767c1003)


## 🔧 Kurulum (VS Code ile)

1. **VS Code ile proje klasörünü açın**  
   `File > Open Folder` menüsünden proje klasörünü seçin.

2. **Terminali açın**  
   Menüden `Terminal > New Terminal` seçeneğini kullanarak terminali başlatın.

3. **.NET SDK’nın kurulu olduğunu kontrol edin**  
   Terminale şu komutu yazın:  
   ```bash
   dotnet --version
Bu komut geçerli bir sürüm döndürmelidir (örneğin: 6.0.100).

Projeyi derleyin
Terminalde aşağıdaki komutu çalıştırın:


dotnet build
Uygulamayı çalıştırın
Derleme başarılıysa, şu komutla uygulamayı başlatın:


dotnet run
Tarayıcıda görüntüleyin

Terminal çıktısında yer alan http://localhost:xxxx veya https://localhost:xxxx bağlantısını kopyalayıp tarayıcıya yapıştırın.
Örneğin:
https://localhost:7183/index.html










