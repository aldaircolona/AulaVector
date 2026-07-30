## Arquitectura y Estructura del Proyecto: Aula Vector

[cite_start]La solución de la tienda virtual Aula Vector está estructurada bajo una arquitectura modular separada en múltiples bibliotecas de clases y un proyecto principal[cite: 17]. [cite_start]Esta organización permite dividir las responsabilidades de manera limpia, profesional y escalable[cite: 18, 59].

### 1. Descripción de las Capas (Proyectos)

El sistema se compone de los siguientes cuatro proyectos:

- [cite_start]**`AulaVector.Models` (Biblioteca de Clases):** Representa la capa del dominio y entidades[cite: 21]. [cite_start]Contiene las clases fundamentales del negocio (como `Product`, `Order`, `OrderDetail` y `ApplicationUser`) que serán reutilizadas de forma transversal por las demás capas de la aplicación[cite: 21, 153, 161].
- [cite_start]**`AulaVector.Data` (Biblioteca de Clases):** Actúa como la capa de persistencia para el acceso a datos[cite: 20]. [cite_start]Es la encargada de alojar el `ApplicationDbContext`, las configuraciones de Entity Framework Core (bajo el enfoque Database First/Scaffolding con PostgreSQL) y el historial de migraciones[cite: 20, 86, 162].
- [cite_start]**`AulaVector.Utils` (Biblioteca de Clases):** Funciona como la capa de herramientas transversales[cite: 22]. [cite_start]Aquí residen los servicios esenciales que no tienen interfaz gráfica pero son vitales para el backend, como el manejo de archivos locales (`FileService`) y el envío de correos electrónicos (`EmailSender`)[cite: 50, 51].
- [cite_start]**`AulaVector.Web` (Proyecto MVC):** Es la capa de presentación principal[cite: 19]. [cite_start]Contiene la interfaz de usuario, incluyendo los controladores, las vistas `.cshtml` y la lógica de enrutamiento necesaria para interactuar con los clientes y administradores[cite: 19, 61].

### 2. Lineamientos de Dependencias

Para garantizar un flujo correcto de la información y respetar la separación de la arquitectura en capas, las referencias entre proyectos (dependencias) se establecen de la siguiente manera:

- [cite_start]**`AulaVector.Data`** necesita conocer y referenciar a **`AulaVector.Models`**[cite: 25].
- [cite_start]**`AulaVector.Web`** necesita conocer y referenciar a **`AulaVector.Data`**, **`AulaVector.Models`** y **`AulaVector.Utils`**[cite: 25].

---

### 3. Estructura Interna del Proyecto Web (`AulaVector.Web`)

[cite_start]Dentro de la capa de presentación, el proyecto organiza sus controladores y vistas de forma estratégica utilizando Áreas (Areas) para separar la experiencia pública de los portales privados[cite: 58, 60].

#### Controladores Raíz (Área Pública y Ventas)

[cite_start]Representan la cara visible de la tienda digital[cite: 63].

- [cite_start]**`BooksController`:** Garantiza que el cliente busque, descubra y elija un producto digital sin fricciones directamente desde la raíz[cite: 64].
- [cite_start]**`CartController`:** Cubre la gestión temporal de los ítems seleccionados en la sesión del usuario[cite: 65].
- [cite_start]**`CheckoutController`:** Actúa como el conector clave para el proceso de pago mediante pasarelas externas[cite: 66].

#### Área `Customer` (Portal Privado)

[cite_start]Encapsula el área accesible únicamente para los usuarios autenticados a través de ASP.NET Core Identity[cite: 67].

- [cite_start]**`LibraryController`:** Funciona como el panel principal donde el usuario encuentra el historial y acceso a los libros o PDFs que ha adquirido[cite: 68].
- [cite_start]**`ProfileController`:** Espacio ideal para la actualización de datos personales y la gestión de la contraseña[cite: 69].

#### Área `Admin` (Gestión Integral)

[cite_start]Agrupa las funcionalidades exclusivas para los usuarios con el rol de Administrador, permitiendo administrar la plataforma de manera segura[cite: 70].

- [cite_start]**`BooksController` y `CategoriesController`:** Cubren la gestión del catálogo y el CRUD de los productos digitales[cite: 71].
- [cite_start]**`CustomersController`:** Facilita el listado de los usuarios registrados a través del sistema Identity[cite: 72].
- [cite_start]**`OrdersController` y `ReportsController`:** Son los espacios correctos para generar reportes visuales o listar las órdenes procesadas en el sistema[cite: 73].
