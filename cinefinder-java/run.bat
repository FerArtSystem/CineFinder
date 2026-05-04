@echo off
:: Inicia a API Java Spring Boot usando o JDK e Maven embutidos no Apache NetBeans
set "JAVA_HOME=C:\Program Files\Apache NetBeans\jdk"
set "PATH=%JAVA_HOME%\bin;C:\Program Files\Apache NetBeans\java\maven\bin;%PATH%"
echo [CineFinder Java] Iniciando Spring Boot na porta 8080...
mvn spring-boot:run
