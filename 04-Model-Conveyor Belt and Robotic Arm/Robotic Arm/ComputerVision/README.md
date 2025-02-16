# TensorFlow GPU Setup on WSL

This guide explains how to set up a Python environment with TensorFlow GPU support on Windows Subsystem for Linux (WSL).

## Prerequisites

- Windows 10/11 with WSL support
- NVIDIA GPU
- NVIDIA drivers installed on Windows

## Installation Steps

### 1. WSL Setup

Install Ubuntu 20.04 on WSL:
```bash
wsl --install -d Ubuntu-20.04
wsl --set-version Ubuntu-20.04 2
Verify WSL version:
wsl --version

### 2. NVIDIA Driver Verification

After installing NVIDIA drivers in Windows, verify the installation in WSL:
nvidia-smi

### 3. Conda Installation

Download and install Miniconda:
wget https://repo.anaconda.com/miniconda/Miniconda3-latest-Linux-x86_64.sh
bash Miniconda3-latest-Linux-x86_64.sh
source ~/.bashrc

### 4. Environment Setup
Create and activate a new Conda environment:
conda create -n robotic-arm python=3.10
conda activate robotic-arm

### 5. CUDA and cuDNN Installation
Install CUDA toolkit and cuDNN:
conda install -c conda-forge cudatoolkit=11.8.0
conda install -c conda-forge cudnn=8.9.2.26

### 6. Environment Variables
Set up required environment variables:
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$CONDA_PREFIX/lib/
echo 'export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$CONDA_PREFIX/lib/' >> ~/.bashrc
source ~/.bashrc

### 7. TensorFlow Installation
# Install TensorFlow
pip install tensorflow==2.12

# Install Robotics Toolbox and dependencies
pip install spatialmath-python==1.0.0
pip install spatialgeometry==1.0.3
pip install roboticstoolbox-python==1.0.3
pip install qpsolvers[open_source_solvers]

# Install Scientific Computing Packages
conda install pandas
conda install scikit-learn
conda install opencv-python
conda install matplotlib