# 🛒 ShopApp

Eine App um Produkte zu kaufen und Bestellungen zu tätigen.
Besteht aus einer API und einer WPF-Anwendung.

**Hinweis:** Einige Funktionen sind noch nicht verfügbar, z.B. das Wechseln seines Passworts aber es ist bereits in der API drin oder das Speichern von Anmeldedaten.
             Es gibt eine Verbesserungsmöglichkeiten, wie z.B. das Cachen von Produktinformationen um nicht jedes mal eine API Request schicken zu müssen.

Alle benutzten Bilder sind von https://icons8.de/.

##TODO:
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
![login](https://github.com/user-attachments/assets/0cb7d8a9-e143-4635-ad9d-c3dae1c8549e)
![register](https://github.com/user-attachments/assets/c2c61292-da98-403f-98fb-5443f5c8ec6a)
![produkte](https://github.com/user-attachments/assets/ec52c875-383d-415e-acdf-31ef102b1fb6)
![warenkorb](https://github.com/user-attachments/assets/5c6ce195-07e0-4328-beb6-e9c183384f39)
![bestellungen](https://github.com/user-attachments/assets/956046d5-4f79-4941-a98f-9d29fac0127b)
