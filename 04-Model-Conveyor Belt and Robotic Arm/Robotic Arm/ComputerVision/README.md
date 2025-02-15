# Stock Data Pipeline

A Python-based data pipeline that fetches stock data from Twelve Data API, stores it in Google Sheets, and performs analysis using BigQuery.

## Features

- Fetch real-time and historical stock data from Twelve Data API
- Store data in Google Sheets for easy visualization and sharing
- Perform advanced analysis using BigQuery
- Comprehensive error handling and logging
- Modular and maintainable code structure

## Prerequisites

- Python 3.8 or higher
- Google Cloud Platform account with BigQuery enabled
- Google Service Account with appropriate permissions
- Twelve Data API key

## Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/stock-data-pipeline.git
cd stock-data-pipeline
```

2. Create and activate a virtual environment:
```bash
python -m venv robotic-arm       # python -m venv robotic-arm-window
source robotic-arm/bin/activate  # On Windows use: robotic-arm-window\Scripts\activate

or 
pip install poetry
poetry init # Starting a new project
poetry add requests # Add dependencies to your project (e.g., requests)
poetry install --no-root # install dependencies and create the virtual environment
poetry shell # Activate the virtual environment
exit # Exit the virtual environment

# Check the virtual environment information
poetry env info 
# Reinstall dependencies
poetry lock --no-update
poetry install --no-root

```

3. Connect to local Unity Server on the port of 55001
    - Open Command Prompt (Windows) or Terminal (Linux/Mac)
    - Run the command to check if the Unity Server is running: 
        Windows: netstat -an | find "55001"
        Linux/Mac: netstat -an | grep 55001
    - Windows and WSL operate in different network namespaces, thus the server running on Windows (port 55001) is not visible to WSL. To connect to the Unity Server from WSL, you need to use default gateway IP in WSL by the command.
        WSL: ip route | grep default (e.g., default via 172.22.96.1 dev eth0 proto kernel)
        Command Prompt: ipconfig (Ethernet adapter vEthernet (WSL (Hyper-V firewall)):  -> IPv4 Address)
    - Then, you can use the default gateway IP to connect to the Unity Server from WSL by running the following command:
        WSL: curl <default gateway>:<port> (e.g., curl http://172.22.96.1:55001)
    - If the connection is unsuccessful, you may need to temporarily turn off the firewall settings by the command
        Windows (Command Prompt as Admin): netsh advfirewall set allprofiles state off

    In case of the fact that the solution above does not work, you can use the following command to connect to the Unity Server from WSL. This proxies requests to 10.255.255.254:55001 to 127.0.0.1:55001.
        Window (Command Prompt (Admin)) 
            netsh interface portproxy add v4tov4 listenaddress=10.255.255.254 listenport=55001 connectaddress=127.0.0.1 connectport=55001
            netsh interface portproxy add v4tov4 listenaddress=172.22.96.1 listenport=55001 connectaddress=127.0.0.1 connectport=55001

