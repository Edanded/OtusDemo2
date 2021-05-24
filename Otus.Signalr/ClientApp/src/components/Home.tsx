
import axios from 'axios';
import * as React from 'react';
import ChatRoom from './ChatRoom';

const Home: React.FC<any> = () => {

  const [login, setLogin] = React.useState<string | undefined>(undefined);
  const [token, setToken] = React.useState<string | undefined>(undefined);
  const [isAuth, setIsAuth] = React.useState<boolean>(false);

  const onSubmitLogin = async (ev: any) => {
    ev.preventDefault();
    const data = await axios.post<string>('http://localhost:5000/api/chat/login', { login: login });
    setToken(data.data);
    setIsAuth(true);
  };

  const onLoginChange = (ev: any) => {
    setLogin(ev.currentTarget.value);
  };

  const loginBlock = () => {
    return <>
      <form onSubmit={onSubmitLogin}>
        <label style={{ display: 'block' }}>Введите логин</label>
        <input type="text" onChange={onLoginChange} />
        <input type="submit" value="Войти" />
      </form>
    </>;
  };

  return !isAuth ?
    loginBlock() :
    <ChatRoom token={token!} login={login!} />;

};

export default Home;
