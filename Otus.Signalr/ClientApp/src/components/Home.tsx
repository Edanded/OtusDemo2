
import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import ChatRoom from './ChatRoom';

const Home = () => {

  const [isAuth, setIsAuth] = React.useState<boolean>(false);
  const [login, setLogin] = React.useState<string | undefined>(undefined);
  




  const onSubmitLogin = (data: any) => {
    setIsAuth(true);
  };

  const loginBlock = () => {
    return <>
      <form onSubmit={onSubmitLogin}>
        <label style={{ display: 'block' }}>Введите логин</label>
        <input type="text" onChange={(ev) => setLogin(ev.target.value)} />
        <input type="submit" value="Войти" />
      </form>
    </>;
  };






  return !isAuth ? loginBlock() : <ChatRoom login={login!}/>;

};

export default connect((state: ApplicationState) => state.isAuthenticated)(Home);
