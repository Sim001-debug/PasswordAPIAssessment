# 🔐 Password API Assessment

This project is part of a technical assessment for Warp Development. 
It consists of three main components working together to crack a password using a dictionary attack and submit the results via a base64-encoded zip file.

---

## Project Overview

### Goal
- Generate all possible permutations of the word `"password"` using common character substitutions.
- Attempt to authenticate each generated password using Basic Auth.
- On successful authentication, receive an upload URL.
- Zip the necessary files (including your CV), encode it to base64, and upload it to the received URL.

---

## Project Structure

```plaintext
PasswordAPIAssessment/
│
├── Program.cs              # Entry point of the application
├── PasswordGenerator.cs    # Generates all password permutations (dict.txt)
├── PasswordCracker.cs      # Attempts login using Basic Auth with each password
├── Uploader.cs             # Zips required files and uploads them as base64 JSON
├── dict.txt                # Dictionary file containing generated passwords
├── submission.zip          # Final zip file to be uploaded


---

## How It Works

### 1.PasswordGenerator
Generates all permutations of the word `"password"` using common substitutions:
- `a` → `a`, `A`, `@`
- `s` → `s`, `S`, `5`
- `o` → `o`, `O`, `0`
- `p` → `p`, `P`
- `w` → `w`, `W`
- `r` → `r`, `R`
- `d` → `d`, `D`

> Output: `dict.txt` – a dictionary file with all permutations

---

### 2.PasswordCracker
- Reads each password from `dict.txt`
- Sends a GET request using **HTTP Basic Authentication**
- If a valid password is found, it returns an **upload URL** provided in the response

---

### 3.Uploader
- Zips the following files into `submission.zip`:
  - `Program.cs`
  - `PasswordGenerator.cs`
  - `PasswordCracker.cs`
  - `Uploader.cs`
  - `dict.txt`
  - Your CV
- Converts the ZIP to a **Base64** string
- Sends a POST request with the payload:
```json
{
  "Data": "<Base64 ZIP>",
  "Name": "Simbongile",
  "Surname": "Dyi",
  "Email": "nwabisamxa@gmail.com"
}

How to Run It
Clone the repo or open in your IDE

Ensure your CV is placed at the correct path (can be hardcoded or passed in)

Run the project

On success, the app will:

Print a valid upload URL

Upload the zip

Confirm with a success message

🧰 Technologies Used
C# (.NET 8)

System.Net.Http

System.IO.Compression

System.Text.Json

Console App

👨‍💻 Author
Simbongile Dyi
Simbongile.Dyi99@gmail.com
💼 LinkedIn : https://www.linkedin.com/in/simbongile-dyi-288227249/

Thank you for the opportunity – I enjoyed this challenge! :) ;)