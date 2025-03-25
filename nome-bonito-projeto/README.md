# 📡 Projeto de Integração de Sistemas com Kafka, Spring Boot, .NET e Angular

Este projeto demonstra a integração entre duas APIs (Java e C#) usando **Apache Kafka** como barramento de mensagens, com um **frontend Angular** para interação do usuário. Tudo é orquestrado com **Docker Compose**.

---

## 🧩 Tecnologias Utilizadas

- **Spring Boot (Java)** → API produtora (Producer)
- **.NET (C# Minimal API)** → API consumidora (Consumer)
- **Apache Kafka + Zookeeper** → Mensageria
- **Kafka UI** → Interface visual para monitoramento dos tópicos Kafka
- **Angular 17** → Frontend standalone
- **Docker & Docker Compose** → Orquestração dos serviços

---

## 🔁 Funcionamento

1. O usuário preenche um nome no Angular (`frontend`)
2. O nome é enviado para a **API Java (`producer`)**
3. A API Java publica esse nome em um tópico Kafka
4. A **API .NET (`consumer`)** consome o nome e processa
5. O resultado pode ser consultado via frontend

---

## ▶️ Como Executar

### ✅ Pré-requisitos

- Docker instalado ([link](https://docs.docker.com/get-docker/))
- Docker Compose instalado

### 🛠️ Passos para rodar o projeto

```bash
# Clone o projeto
git clone https://github.com/igorsouzaccruz/projeto_tec_de_integracao_de_sistemas


# (Opcional) Remova containers antigos
docker-compose down --volumes

# Suba tudo 
docker-compose up 
```
### 🌐 Endpoints e Acessos
```
Serviço	URL
Frontend	http://localhost:4200
API Producer	http://localhost:8082/api1
API Consumer	http://localhost:8083/api2
Kafka UI	http://localhost:8080
```

---

#### 📁 Estrutura do Projeto
```
bash
Copiar
Editar
├── api1/         # Spring Boot Producer
├── api2/         # .NET Minimal API Consumer
├── frontend/     # Angular 17 (standalone)
├── docker-compose.yml
└── README.md
```