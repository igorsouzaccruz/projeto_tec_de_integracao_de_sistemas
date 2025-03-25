package com.example.api1.config;

import org.apache.kafka.clients.admin.NewTopic;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.kafka.config.TopicBuilder;

@Configuration
public class KafkaConfig {
    @Bean
    public NewTopic topicVerificarNome() {
        return TopicBuilder.name("topic-verificar-nome").build();
    }

    @Bean
    public NewTopic topicNomesBonitos() {
        return TopicBuilder.name("topic-nomes-bonitos").build();
    }
}
