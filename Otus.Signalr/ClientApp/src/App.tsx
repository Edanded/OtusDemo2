import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';


import './custom.css'

export default () => (
    <Layout>
        <Route path='/' component={Home} />
    </Layout>
);
