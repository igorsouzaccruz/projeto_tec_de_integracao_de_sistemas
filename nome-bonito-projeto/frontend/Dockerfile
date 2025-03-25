# Etapa 1: build da aplicação
FROM node:20 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build -- --output-path=dist

# Etapa 2: imagem leve com NGINX para servir a aplicação
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
