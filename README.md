# SoftRenderer

## Overview

This project is a 3D rendering engine implemented in pure C#, currently supporting **rasterization** as the main rendering technique. The goal is to provide an efficient and flexible renderer that can showcase real-time graphics using a rasterizer.

### Key Features

- **Rasterizer**: Implements a real-time renderer using techniques such as z-buffering, shading, and texturing.
- **Model Upload**: Supports loading 3D models in `.obj` format for visualization. Uploaded models are automatically rendered using the rasterizer.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Renderer Features](#renderer-features)
- [Contributing](#contributing)

## Installation

### Prerequisites

- [.NET FrameWork SDK 4.8 or higher](https://dotnet.microsoft.com/download/dotnet/6.0)
- StyleCop Analyzer 1.11.8 or higher
- .NET FrameWork SDK 4.8 developer pack
- A text editor or IDE (Visual Studio or JetBrains Rider recommended)

### Steps to Install

1. Clone the repository:

    ```bash
    git clone https://github.com/philimon-reset/SoftRenderer.git
    cd SoftRenderer
    ```

2. Restore the .NET packages:

    ```bash
    dotnet restore
    ```

3. Build the project:

    ```bash
    dotnet build
    ```

4. Run the project:

    ```bash
    dotnet run
    ```

## Future Usage

### Running the Rasterizer  (In Progress)

The rasterizer renders the scene in real-time. By default, uploaded `.obj` files are automatically rendered using the rasterizer.

```bash
dotnet run -- -rasterize
```

### Configuration Options

- **Input Model**: Provide a `.obj` file to load a custom 3D model.

    ```bash
    dotnet run -- -rasterize -input model.obj
    ```

- **Resolution**: Specify output resolution.

    ```bash
    dotnet run -- -rasterize -resolution 1920x1080
    ```

## Renderer Features

### Rasterizer

- **Z-Buffering**: Depth-buffering technique to determine visible surfaces.
- **Shading Models**: Flat shading, Gouraud shading, Phong shading.
- **Texturing**: Support for texture mapping using UV coordinates.
- **Culling**: Back-face culling to optimize rendering performance.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

### Development Guidelines

- Document your code using StyleCop comments.

### Future Improvements

- **Ray Tracer**: Add support for a path-tracing ray tracer with features like reflections, refractions, shadows, and global illumination.
- **Material Support**: Implement different materials like Lambertian, Phong, and specular reflection models.
- **Lighting Models**: Support point, directional, and ambient lighting.
- **Anti-aliasing**: Add supersampling for smoother images.
- **Multithreading**: Parallel rendering for performance optimization.
- **Support for OBJ Files**: Add support for loading 3D models from `.obj` files.
- **GPU Acceleration**: Implement GPU-accelerated rendering (using Vulkan or DirectX).
