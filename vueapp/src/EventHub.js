import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'
export default {
    install(Vue) {
        const connection = new HubConnectionBuilder()
            .withUrl('http://localhost:7282/EventHub')
            .configureLogging(LogLevel.Information)
            .build()

        connection.start()
    }
}