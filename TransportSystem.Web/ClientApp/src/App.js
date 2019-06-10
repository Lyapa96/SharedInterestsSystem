import React from 'react';
import { Route } from 'react-router-dom';
import Layout from './components/Layout';
import Home from './components/Home';
import Info from './components/Info';
import Passengers from './components/Passengers';
import Smo from './components/Smo'

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/info' component={Info} />
    <Route path='/passengers' component={Passengers} />
    <Route path='/smo' component={Smo} />
  </Layout>
);
