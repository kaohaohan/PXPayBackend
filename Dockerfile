# Stage 1: Build（编译阶段）
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 复制项目文件
COPY ["PXPayBackend.csproj", "./"]
RUN dotnet restore "PXPayBackend.csproj"

# 复制所有代码并编译
COPY . .
RUN dotnet build "PXPayBackend.csproj" -c Release -o /app/build
RUN dotnet publish "PXPayBackend.csproj" -c Release -o /app/publish

# Stage 2: Runtime（运行阶段）
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# 从 build 阶段复制编译好的文件
COPY --from=build /app/publish .

# 暴露端口
EXPOSE 5000

# 设定环境变量
ENV ASPNETCORE_URLS=http://+:5000

# 启动命令
ENTRYPOINT ["dotnet", "PXPayBackend.dll"]

