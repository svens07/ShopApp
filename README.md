# 🛒 ShopApp

Eine App um Produkte zu kaufen und Bestellungen zu tätigen.
Besteht aus einer API und einer WPF-Anwendung.

**Hinweis:** Einige Funktionen sind noch nicht verfügbar, z.B. das Wechseln seines Passworts aber es ist bereits in der API drin oder das Speichern von Anmeldedaten.
             Es gibt eine Verbesserungsmöglichkeiten, wie z.B. das Cachen von Produktinformationen um nicht jedes mal eine API Request schicken zu müssen.

Alle benutzten Bilder sind von https://icons8.de/.

### TODO:
- Detailierte Bestellinformationen (z.B. Adresse, Name, etc)
- Noch zu fehlende Funktionen hinzufügen

---
### **API**
Die API ist da um Benutzerkonten, Warenkörbe, Bestellungen und Produkte zu verwalten.

#### **AuthController (`/auth`)**
- **`/login`**: Anmeldung eines bestehenden Accounts. 
- **`/register`**: Registrierung eines neuen Accounts  
- **`/change-pass`**: Passwort eines Accounts ändern  
- **`/me`**: Informationen über den aktuellen Benutzer abrufen

#### **CartController (`/cart`)**
- **`/items`**: Gibt alle Produkte im Warenkorb des Nutzers zurück  
- **`/add`**: Fügt ein Produkt in den Warenkorb ein  
- **`/remove`**: Entfernt ein Produkt aus dem Warenkorb 
- **`/modify`**: Ändert die Anzahl eines Produkts im Warenkorb

#### **OrderController (`/order`)**
- **`/create`**: Erstellt eine neue Bestellung aus den aktuellen Warenkorbdaten
- **`/list`**: Gibt eine Liste aller Bestellungen des Nutzers zurück

#### **ProductController (`/product`)**
- **`/list`**: Gibt eine Übersicht über alle verfügbaren Produkte  
- **`/info`**: Liefert detaillierte Informationen zu einem spezifischen Produkt
---

### **Struktur der Datenbank**
- **users**: id, username, email, passwordHash, role, createdAt, lastLogin, isEnabled
- **products**: id, name, iamge, category, description, price, isEnabled, createdAt
- **orders**: id, userId, cartId, totalAmount, createdAt
- **carts**: id, userId, isActive, createdAt, totalAmount
- **cart_items**: id, cartId, productId, quantity, updatedAt, createdAt


### **Bilder**
![login](https://github.com/user-attachments/assets/93ba9059-24f9-4634-a5b6-31ee560a0929)
![register](https://github.com/user-attachments/assets/75c9a645-cfd4-4370-9b3b-ef5c24aa64de)
![produkte](https://github.com/user-attachments/assets/07883f8c-d8ee-4290-9854-b259e96d7147)
![warenkorb](https://github.com/user-attachments/assets/007fe998-86eb-4946-8e8c-cc2539ba52da)
![bestellungen](https://github.com/user-attachments/assets/be5c2e69-1fc9-46d0-b6f7-55ef439aea6e)
