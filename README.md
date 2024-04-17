# eCinemax

### Technologies

#### Backend
   - [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
   - [Mongo](https://www.mongodb.com/)
   - [Hangfire](https://www.hangfire.io/)
   - MediatR
   - FluentValidation
   - Vertical Slice Architecture
   - CQRS Pattern, REPR Pattern
#### Frontend
   - React Native (Expo SDK 50.0.17)
   - Axios, Redux Toolkit, Redux Thunk
   - [Native Wind 2.0.11](https://www.nativewind.dev/)
#### Development Tools
   - [Docker](https://www.docker.com/)
   - [Ngrok](https://ngrok.com/)

### Setup
#### At project root directory, run environment command
    docker-compose -f ./docker-compose.env.yml up -d

#### At ./eCinemax/src/eCinemax.Server
    dotnet restore
######
    dotnet run

#### At ./eCinemax/src/eCinemax.Client
    Create .env
    EXPO_PUBLIC_BASE_URL=http://YOUR_IP_ADDRESS:5015
    Replace 'YOUR_IP_ADDRESS' with your local IP address
######
    npm i
######
    npm start

#### Bonus: If you want to run the project on your phone

    docker run -it -e NGROK_AUTHTOKEN=2cf3AoXSrJ3zlEvQO03JSWmkcEU_7zhDmDadCLCpyJmP4BJjo ngrok/ngrok:latest http --domain=exciting-snail-new.ngrok-free.app 5005

- Backend will listen in exciting-snail-new.ngrok-free.app

- Change the EXPO_PUBLIC_BASE_URL=exciting-snail-new.ngrok-free.app
