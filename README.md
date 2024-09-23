# SoftRenderer

## Overview

This project is a 3D rendering engine implemented in pure C#, which supports both **rasterization** and **ray tracing** rendering techniques. The goal is to provide an efficient and flexible renderer that can showcase real-time graphics using a rasterizer and high-quality offline rendering with a ray tracer.

### Key Features

- **Rasterizer**: Implements a real-time renderer using techniques such as z-buffering, shading, and texturing.
- **Ray Tracer**: Implements a path-tracing ray tracer supporting reflections, refractions, shadows, and global illumination.
- **Material Support**: Supports different materials like Lambertian, Phong, and specular reflection models.
- **Lighting**: Implements point, directional, and ambient lighting models.
- **Anti-aliasing**: Optional supersampling for smoother images.
- **Scene Parsing**: Load 3D models from OBJ files.
- **Multithreading**: Parallel rendering using multithreading for performance optimization in ray tracing.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Renderer Features](#renderer-features)
- [Contributing](#contributing)
- [License](#license)

## Installation

### Prerequisites

- [.NET Core SDK 6.0 or higher](https://dotnet.microsoft.com/download/dotnet/6.0)
- A text editor or IDE (Visual Studio or JetBrains Rider recommended)

### Steps to Install

1. Clone the repository:

    ```bash
    git clone https://github.com/philimon-reset/3D-Renderer.git
    cd 3D-Renderer
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

## Usage

### Running the Rasterizer

The rasterizer renders the scene in real-time. You can run it by specifying the `-rasterize` flag.

```bash
dotnet run -- -rasterize
```

### Running the Ray Tracer

The ray tracer provides high-quality, realistic images. Use the `-raytrace` flag to run it.

```bash
dotnet run -- -raytrace
```

### Configuration Options

- **Input Model**: Provide a `.obj` file to load a custom 3D model.

    ```bash
    dotnet run -- -rasterize -input model.obj
    ```

- **Resolution**: Specify output resolution.

    ```bash
    dotnet run -- -raytrace -resolution 1920x1080
    ```

- **Samples per pixel**: For the ray tracer, adjust the number of samples per pixel to improve image quality (anti-aliasing).

    ```bash
    dotnet run -- -raytrace -samples 100
    ```

- **Multithreading**: Control the number of threads used for rendering.

    ```bash
    dotnet run -- -raytrace -threads 8
    ```

## Renderer Features

### Rasterizer
- **Z-Buffering**: Depth-buffering technique to determine visible surfaces.
- **Shading Models**: Flat shading, Gouraud shading, Phong shading.
- **Texturing**: Support for texture mapping using UV coordinates.
- **Culling**: Back-face culling to optimize rendering performance.
  
### Ray Tracer
- **Shadows**: Accurate shadow calculation using ray intersections.
- **Reflections**: Specular reflections based on material properties.
- **Refractions**: Transparent materials with customizable index of refraction.
- **Global Illumination**: Simulates light bouncing for more realistic lighting.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

### Development Guidelines

- Document your code using StyleCop comments.

### Future Improvements

- Add support for complex lighting models like PBR (Physically Based Rendering).
- Support GPU-accelerated rendering (using Vulkan or DirectX).

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.
