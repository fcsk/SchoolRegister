export default class BaseService {
    constructor(domain) {
        var hostUrl = undefined;
        if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
            hostUrl = window.location.protocol +
                "//" +
                window.location.hostname +
                ":" +
                window.location.port +
                '/api';
        } else {
            hostUrl = process.env.PUBLIC_URL + '/api';
        }
        this.domain = domain || hostUrl;
        this.fetch = this.fetch.bind(this);
        this.isSuccess = true;
    }

    fetch(url, options, token) {

        const headers = {
            'Accept': 'text/plain'
        };

        if (token !== null) {
            headers['Authorization'] = 'Bearer ' + token;
        }
        var localThis = this;
        return fetch(url,
            {
                headers,
                ...options
            })
            .then(response => { return this._checkStatus(response, localThis); })
            .then(response => {
                Promise.resolve(response);
                if (response !== null && response !== undefined) {
                    localThis.isSuccess = true;
                    return response.json();
                }
            });

    }

    _checkStatus(response, localThis) {
        // raises an error in case response status is not a success
        if (!response.ok) {
            const error = new Error(response.statusText);
            error.response = response;
            localThis.isSuccess = false;
            response.text().then(function (err) {
                alert(err);
            });
        }
        if (response.status >= 200 && response.status < 300) { // Success status lies between 200 to 300
            return response;
        }
    }
}