import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import withAuth from './components/Authentication/withAuth';
import { FetchData } from './components/FetchData';
import { Subject } from './components/Subject';
class App extends Component {
    displayName = App.name
    componentDidMount() {
    }

    render() {
        return (
            <div>
                <Layout history={this.props.history} user={this.props.user}>
                    <Route exact path='/' component={FetchData} />
                    <Route exact path='/fetchdata' component={FetchData} />
                </Layout>

            </div>
        );
    }
}
export default withAuth(App);