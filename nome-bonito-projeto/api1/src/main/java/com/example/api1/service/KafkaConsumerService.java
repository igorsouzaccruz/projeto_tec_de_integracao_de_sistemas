package com.example.api1.service;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.nio.charset.StandardCharsets;
import java.security.NoSuchAlgorithmException;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.stereotype.Service;

@Service
public class KafkaConsumerService {

    private final ObjectMapper objectMapper = new ObjectMapper();

    // Guardar todos os nomes e se são bonitos
    private final Map<String, Boolean> nomesBonitos = new ConcurrentHashMap<>();

    @KafkaListener(topics = "topic-nomes-bonitos", groupId = "grupo1")
    public void receberResultado(String mensagemJson) {
        try {
            NomeBonito recebido = objectMapper.readValue(mensagemJson, NomeBonito.class);
            nomesBonitos.put(recebido.getNome().toLowerCase(), recebido.isBonito());

            String esperado = gerarSha256("igor-e-lucas");

            if (!esperado.equals(recebido.getToken())) {
                System.out.println("❌ Token inválido. Ignorando mensagem.");
                return;
            }

            System.out.println("✅ Nome armazenado: " + recebido.getNome() + " | Bonito: " + recebido.isBonito());
        } catch (Exception e) {
            System.err.println("❌ Erro ao processar mensagem Kafka: " + e.getMessage());
        }
    }

    public boolean isNomeBonito(String nome) {
        return nomesBonitos.getOrDefault(nome.toLowerCase(), false);
    }

    // Classe auxiliar para desserializar JSON
    public static class NomeBonito {
        private String nome;
        @JsonProperty("ehBonito")
        private boolean bonito;
        private String token;

        public String getNome() { return nome; }
        public void setNome(String nome) { this.nome = nome; }

        public boolean isBonito() { return bonito; }
        public void setBonito(boolean bonito) { this.bonito = bonito; }

        public String getToken() { return token; }
        public void setToken(String token) { this.token = token; }
    }

    public String gerarSha256(String input) {
    try {
        MessageDigest digest = MessageDigest.getInstance("SHA-256");
        byte[] encodedHash = digest.digest(input.getBytes(StandardCharsets.UTF_8));
        StringBuilder hexString = new StringBuilder();

        for (byte b : encodedHash) {
            String hex = Integer.toHexString(0xff & b);
            if (hex.length() == 1) hexString.append('0');
            hexString.append(hex);
        }

        return hexString.toString(); // minúsculas, igual ao C#
    } catch (NoSuchAlgorithmException e) {
        throw new RuntimeException(e);
    }
}
}