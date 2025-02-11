#!/bin/bash

# Function to start Ollama server if not already running
start_ollama_server() {
  echo "Starting Ollama server..."
  ollama serve &
  pid=$!  # Store the process ID of the Ollama server
}

# Function to wait for Ollama server to be active
wait_for_ollama_server() {
  echo "Waiting for Ollama server to be active..."
  while [ "$(ollama list | grep 'NAME')" == "" ]; do
    sleep 1
  done
  echo "Ollama server is now active."
}

# Function to pull models
pull_model() {
  local model_name=$1
  echo "?? Retrieve $model_name model..."
  ollama pull $model_name
  echo "?? Done!"
}

# Main script execution
start_ollama_server
wait_for_ollama_server

# Pull models
pull_model "mxbai-embed-large"
pull_model "qwen2:0.5b"

# List models
ollama list

# Wait for Ollama server process to complete
wait $pid
