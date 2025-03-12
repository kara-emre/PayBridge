# PayBridge

PayBridge is a comprehensive payment system integration solution designed to streamline and simplify the process of handling payments. This project aims to provide a robust and scalable architecture for managing various payment-related operations, ensuring reliability and efficiency.

## Features

- **Payment System Integration**: Seamlessly integrates with multiple payment gateways to facilitate smooth transactions.
- **Order Management**: Efficiently handles order creation, validation, and completion processes.
- **Error Handling**: Implements robust error handling mechanisms to ensure system stability and reliability.
- **Retry Policies**: Utilizes Polly for implementing retry policies, ensuring resilience against transient faults.
- **Asynchronous Messaging**: Leverages MassTransit for handling asynchronous messaging and communication between services.
- **Containerization**: Supports Docker for containerized deployment, ensuring consistency across different environments.

## Technologies Used

- **C#**: Core programming language used for developing the application.
- **ASP.NET Core**: Framework for building web APIs and handling HTTP requests.
- **MassTransit**: Library for managing asynchronous messaging and communication.
- **Polly**: Library for implementing resilience and transient fault handling policies.
- **RestSharp**: Library for making HTTP requests and handling responses.
- **Docker**: Platform for containerizing applications and managing containerized environments.
- **RabbitMQ**: Message broker for handling message queues and ensuring reliable communication between services.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [RabbitMQ](https://www.rabbitmq.com/download.html)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/kara-emre/PayBridge.git
   cd PayBridge
   ```

2. Build and run the Docker containers:
   ```bash
   docker-compose up --build
   ```

3. Ensure RabbitMQ is running and accessible.

### Usage

- Use Swagger to interact with the API. (http://localhost:8080/index.html)
- To create an order, send a POST request to the `/api/create` endpoint with the order details.
- The system will handle the order creation, validation, and payment processing.
- Use the returned Id value from the `/api/create` request and send a request to `/api/orders/{id}/complete` to complete the order.
- Check the logs for detailed information on the processing status and any errors encountered.

 
