import React from 'react';
import ReactDOM from 'react-dom';
// import registerServiceWorker from './registerServiceWorker';
import { unregister } from './registerServiceWorker';

import { BrowserRouter as Router } from 'react-router-dom'
import './assets/base.scss';
import Main from './Pages/Main';
import configureStore from './store/configureStore';
import { Provider } from 'react-redux';

const store = configureStore();
const rootElement = document.getElementById('root');

const renderApp = Component => {
  ReactDOM.render(
    <Provider store={store}>
      <Router>
        <Component />
      </Router>
    </Provider>,
    rootElement
  );
};

renderApp(Main);

if (module.hot) {
  module.hot.accept('./Pages/Main', () => {
    const NextApp = require('./Pages/Main').default;
    renderApp(NextApp);
  });
}
unregister();

// registerServiceWorker();

