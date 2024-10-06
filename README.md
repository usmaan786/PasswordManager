**C# Password Manager**

![Preview of Current App](AN17QCk.png)


This app was created to practice writing C# WPF applications and learn cryptography.

Using AES - 3 locally stored files are created, a key file, iv file and master key file.
- The key and iv files are used to decrypt and encrypt the passwords stored in a json file.
- The master key file protects those two files, they are inaccessible without the master key.

**Features(maybe?) to implement**
- Timestamp the files so they're updated with a new key, iv or master key after every X amount of time on launch (will require decrypting json file and encrypting with new key/iv to keep password data intact)
- Password gate viewing the list of passwords via a popup
- Storing the key and iv through the Cloud
- Browser extension (if Cloud key storage is implemented)
