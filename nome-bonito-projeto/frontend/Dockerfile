# Etapa 1: build da aplicação Angular
FROM node:20 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build 

# Etapa 2: servidor NGINX para servir o build
FROM nginx:alpine
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]