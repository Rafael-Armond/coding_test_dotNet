# CreditosConstituidos – API de Consulta e Processamento de Créditos

Este repositório contém a solução desenvolvida para o **Desafio Técnico – Desenvolvimento de API de Consulta de Créditos**.  
A aplicação implementa uma API RESTful com .NET 6, persistência em PostgreSQL, mensageria com Kafka e um background service responsável pelo processamento assíncrono dos créditos recebidos. Todo o projeto foi arquitetado para rodar em um cotainer docker.

---

## Tecnologias Utilizadas

- **.NET 6 Web API**
- **PostgreSQL 16 (Docker)**
- **Kafka + Zookeeper (Docker)**
- **Entity Framework Core 6**
- **Docker & Docker Compose**
- **MSTest (Testes Unitários)**
- **Swagger**
- **Padrões de Projeto:** Clean Architecture, SOLID, Repository Pattern, Factory Pattern

---

## Arquitetura (Clean Architecture)

CreditosConstituidos.Api -> Controllers, DI, Swagger, Background Service
CreditosConstituidos.Application -> UseCases, DTOs, Mappers (Factory Pattern), Interfaces
CreditosConstituidos.Domain -> Entidades
CreditosConstituidos.Infrastructure -> EF Core, Repositórios, Kafka
CreditosConstituidos.Tests -> Testes unitários

---

# Como Rodar o Projeto com Docker

## 1. Pré-requisitos

- Docker Desktop instalado
- Portas disponíveis: **5000**, **5432**, **9092**, **2181**

---

## 2. Clonar o repositório

```bash
git clone https://github.com/Rafael-Armond/coding_test_dotNet.git
cd coding_test_dotNet
```

## Subir a aplicação com Docker Compose

```bash
docker-compose up --build
```

# Acessar o swagger

Inserir na barra de pesquisa do navegador: http://localhost:5000/swagger

# Endpoints da API

- POST /api/creditos/integrar-credito-constituido
  Publica cada crédito no tópico Kafka integrar-credito-constituido-entry e retorna 202 Accepted.

Exemplo de body:

```json
[
  {
    "numeroCredito": "123456",
    "numeroNfse": "7891011",
    "dataConstituicao": "2024-02-25",
    "valorIssqn": 1500.75,
    "tipoCredito": "ISSQN",
    "simplesNacional": "Sim",
    "aliquota": 5.0,
    "valorFaturado": 30000,
    "valorDeducao": 5000,
    "baseCalculo": 25000
  }
]
```

- GET /api/creditos/{numeroNfse}
  Retorna todos os créditos vinculados à NFSe informada.

Response:

```json
[
  {
    "numeroCredito": "789456",
    "numeroNfse": "7891011",
    "dataConstituicao": "2024-02-25T00:00:00",
    "valorIssqn": 1500.75,
    "tipoCredito": "ISSQN",
    "simplesNacional": "Sim",
    "aliquota": 7,
    "valorFaturado": 30000,
    "valorDeducao": 5000,
    "baseCalculo": 25000
  }
]
```

- GET /api/creditos/credito/{numeroCredito}
  Retorna um crédito específico pelo número do crédito.

```json
{
  "numeroCredito": "789456",
  "numeroNfse": "7891011",
  "dataConstituicao": "2024-02-25T00:00:00",
  "valorIssqn": 1500.75,
  "tipoCredito": "ISSQN",
  "simplesNacional": "Sim",
  "aliquota": 7,
  "valorFaturado": 30000,
  "valorDeducao": 5000,
  "baseCalculo": 25000
}
```

- GET /self
  Health-check básico.

- GET /ready
  Verifica se a API está pronta e conectada ao banco.
