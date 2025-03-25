package com.example.api1.service;

import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.stereotype.Service;

@Service
public class KafkaConsumerService {
    private boolean nomeBonito;

    @KafkaListener(topics = "topic-nomes-bonitos", groupId = "grupo1")
    public void receberResultado(String resultado) {
        this.nomeBonito = Boolean.parseBoolean(resultado);
    }

    public boolean isNomeBonito() {
        return nomeBonito;
    }
}
