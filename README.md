# Encrypt-App

This app is intended as a password manager and caesar cipher for quickly generating new passcodes per website. There are three options available and with different parameters used.

- Website
- Length of password
- Character set
- An secret, either for salting or for caesar shifting

The app is designed to minimize the work of keeping different passwords per website while keeping each website reasonably unique and able to comply with different character sets in a fast manner, without keeping the exact password in plaintext. A hypothetical breach would entail someone downloading all of your files and searching for this program's data specifically, raising the effort required for a high value target. Otherwise, most laymen can enjoy a simplified password manager with no DRM, or network activity required.

Below, there are 3 options provided, where you can create a irreversible and strongly encrypted password. Alternatively, you can use the caesar shift which is weak and easy to convert back to the source input, but may be more flexible to the user's needs.
It is recommended to use the Strong Encrypt on any external and internal resource, whereas the Weak Encrypt may be more appealing for recreational activities and sport.


1. Strong Encrypt

It uses a combination of local file seed, SHA256 integer cycling, and all of the input parameters as salt during password generation.

2. Weak Encrypt

It uses a combination of local file seed and two input parameters, excluding the number of characters specified, as salt during password generation.

3. Decrypt Weak

It uses a combination of local file seed and two input parameters, excluding the number of characters specified, as salt to revert a previously created weak password to plaintext again.


# Backups

Open the folder in the app and copy the `owo` file to cold storage, which is used in the encryption process. Optionally, write any number desired there.

Additionally, the `uwu` folder contains the charset and password length used per website per file. Using a character set native to the app makes it easier on the user. Also copy the `uwu` folder to cold storage.

Afterwards, a consistent input, password length, and character set will guarantee the same result everywhere.

Note, the app only ensures that your password length and character set are the same as previous generations, so you have to memorize the proper input yourself, but it can be the same input on all websites to produce different results.


# Contributions

Pull requests are welcome.


# Warranty and Liability

I accept no liability for what happens. This is FOSS that you use at your own risk.
