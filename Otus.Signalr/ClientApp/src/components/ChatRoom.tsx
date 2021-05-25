import * as React from 'react';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

class ChatMessage {
    message?: string;
    login?: string;
    timestamp?: Date;
};

export class Props {
    login!: string;
    token!: string;
}


const ChatRoom = (props: Props) => {
    const { login, token } = props;
    const [chatHistory, setChatHistory] = React.useState<ChatMessage[]>([]);
    const [connection, setConnection] = React.useState<HubConnection | null>(null);
    const [message, setMessage] = React.useState<string>('');

    const latestChat = React.useRef(null);

    (latestChat as any).current = chatHistory;

    React.useEffect(() => {
        document.title = `Логин: ${login}`;


        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/hubs/chat',
                {
                    transport: HttpTransportType.WebSockets,
                    accessTokenFactory: () => token,

                })
            .withAutomaticReconnect() // Если отвалилось соединение - пытаемся еще раз
            .build();

        setConnection(newConnection);


        if (newConnection) {
            newConnection.start()
                .then(() => {
                    newConnection.on('ReceiveMessage', message => {

                        const updatedChat = [...(latestChat as any).current];
                        updatedChat.push(message);

                        setChatHistory(updatedChat);
                    });
                })
                .catch(e => console.log('Ошибка подключения: ', e));
        }
    }, []);


    const textChange = (ev: any) => {
        setMessage(ev.target.value);
    };

    const sendMessage = async () => {
        await connection!.send('SendMessage', { message, login, timestamp: new Date() });
    };

    return <>
        <div style={{
        display: 'block',
        width: '400px',
        height: '400px',
        overflowY: 'scroll',
        border: '1px solid green',

    }}>{chatHistory.map(x => {
            return <>
                <div key={Date.now() * Math.random()}>
                    <i>{x.login}</i> {x.timestamp}
                    <br />
                    <span>{x.message || ''}</span>
                    <br />
                    <br />
                </div>
            </>;

        })}</div>
        <textarea onChange={textChange}
            style={{ display: 'block', width: '100' }}>

        </textarea>
        <button onClick={sendMessage}>Отправить</button>
    </>;


};

export default ChatRoom;


