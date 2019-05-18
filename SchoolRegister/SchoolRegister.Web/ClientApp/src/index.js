import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { Route } from 'react-router';
import { BrowserRouter as Router } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import Login from './components/Authentication/Login';
import Register from "./components/Authentication/Register";
const baseUrl = !process.env.NODE_ENV || process.env.NODE_ENV === 'development' ? document.getElementsByTagName('base')[0].getAttribute('href') : process.env.PUBLIC_URL;
const rootElement = document.getElementById('root');


ReactDOM.render(
    <Router basename={baseUrl}>
        <div>
            <Route exact path="/" component={App} />
            <Route path="/fetchdata" component={App} />
            <Route exact path="/login" component={Login} />
            <Route exact path="/register" component={Register} />
        </div>
    </Router>,
    rootElement);

registerServiceWorker();