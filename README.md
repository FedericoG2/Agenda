# Agenda de Contactos

## Descripción

Aplicación de escritorio desarrollada en **.NET** y **C#** que permite gestionar y visualizar contactos de manera eficiente. Esta herramienta te permite agrupar contactos por categoría, realizar búsquedas por nombre, teléfono o correo, y modificar la lista de contactos. Además, ofrece funcionalidades para crear, editar, eliminar y exportar la lista de contactos en formatos **CSV** o **vCard**.

## Características

- Visualización de contactos agrupados por categoría.
- Búsqueda de contactos por:
  - Nombre
  - Teléfono
  - Correo
- Gestión de contactos:
  - Crear nuevos contactos
  - Editar contactos existentes
  - Eliminar contactos
- Exportación de la lista de contactos en formatos:
  - CSV
  - vCard

## Base de Datos

La aplicación utiliza una base de datos SQLite que consiste en una única tabla llamada `Contactos`, que contiene los siguientes campos:

- **Nombre**: El primer nombre del contacto.
- **Apellido**: El apellido del contacto.
- **Teléfono**: El número de teléfono del contacto.
- **Correo**: La dirección de correo electrónico del contacto.
- **Categoría**: La categoría bajo la cual se agrupa el contacto (por ejemplo, familia, amigos, trabajo).

## Requisitos

- .NET Framework
- Visual Studio o un IDE compatible con C#
- SQLite

## Instalación

1. Clona el repositorio o descarga el archivo ZIP.
2. Abre el proyecto en Visual Studio.
3. Asegúrate de que todas las dependencias estén instaladas.
4. Compila y ejecuta la aplicación.

## Demo

https://www.loom.com/share/4101983eea3c4585967c771c5cebe49f?sid=74591c49-b8f4-4053-b36b-37561010c0b4

## Contribuciones

Las contribuciones son bienvenidas. Si deseas mejorar la aplicación, por favor crea un fork del repositorio y envía un pull request.

## Licencia

Este proyecto está bajo la [Licencia MIT](LICENSE).
