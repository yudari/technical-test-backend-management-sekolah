# Technical Test Backend Management Sekolah
Repositori ini berisi API RESTful untuk mengelola sistem sekolah berdasarkan soal Technical Test https://docs.google.com/document/d/1JjDoGiJHQfGGIktLK64zjVAl6FDk1SBXBA6NwHD40sg/edit. API ini dibangun menggunakan ASP.NET Core dengan autentikasi berbasis JWT. API ini memiliki fitur operasi CRUD untuk berbagai entitas, seperti siswa, guru, kelas, dan mata pelajaran, dengan mengikuti prinsip Clean Architecture. Koneksi ke basis data menggunakan PostgreSQL.

## Table of Contents

1. [Fitur](#fitur)
2. [Teknologi](#teknologi)
3. [Instalasi](#instalasi)
4. [Konfigutas](#konfigurasi)
5. [API Endpoints](#api-endpoints)
6. [Authentikasi](#authentikasi)
7. [Testing](#testing)
8. [Swagger UI](#swagger-ui)
9. [Contributing](#contributing)
10. [License](#license)

## Fitur

- **CRUD**: Menambahkan, membaca, memperbarui, dan menghapus entitas seperti siswa, guru, kelas, dan mata pelajaran.
- **Autentikasi**: Pengguna dapat masuk menggunakan JWT untuk mendapatkan akses ke API.
- **Role-based Authorization**: Kontrol akses berdasarkan peran pengguna.
- **Swagger UI**: Antarmuka pengguna untuk menguji API secara langsung.

## Teknologi

- **ASP.NET Core**: Framework untuk membangun API.
- **JWT**: Json Web Token untuk autentikasi.
- **PostgreSQL**: Sistem manajemen basis data relasional.
- **Clean Architecture**: Struktur kode yang terorganisir dengan baik untuk pemeliharaan dan skalabilitas.

## Instalasi

1. **Kloning repositori**

   ```bash
   git clone https://github.com/username/repository.git
2. **Masuk ke direktori proyek**
   cd repository

3. 
