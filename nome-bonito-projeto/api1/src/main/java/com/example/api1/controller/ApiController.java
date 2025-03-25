package com.example.api1.controller;

import com.example.api1.service.KafkaProducerService;
import com.example.api1.service.KafkaConsumerService;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api1")
public class ApiController {
    private final KafkaProducerService producerService;
    private final KafkaConsumerService consumerService;

    public ApiController(KafkaProducerService producerService, KafkaConsumerService consumerService) {
        this.producerService = producerService;
        this.consumerService = consumerService;
    }
    
    @GetMapping("/")
    public String status() {
        return "API Java online!";
    }

    @PostMapping("/verificarNome")
    public void verificarNome(@RequestBody String nome) {
        producerService.enviarNomeParaVerificacao(nome);
    }

    @GetMapping("/resultado")
    public boolean obterResultado() {
        return consumerService.isNomeBonito();
    }
}
