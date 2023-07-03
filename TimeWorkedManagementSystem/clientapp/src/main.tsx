import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './satoshi.css';
import './index.css'
import {Auth0Provider} from "@auth0/auth0-react";
import { BrowserRouter as Router } from 'react-router-dom';
import {APIProvider} from "./hooks/useAPI.tsx";

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
      <Auth0Provider
          domain="https://twms.eu.auth0.com"
          clientId="mcUqba2gXvfJbGURt8KmZAHr0RH7yCXN"
          authorizationParams={{
              redirect_uri: window.location.origin,
              audience: "https://twms.app"
          }}
          useRefreshTokens={true}
          useRefreshTokensFallback={true}
          cacheLocation="localstorage"
      >
          <APIProvider>
              <Router>
                <App />
              </Router>
          </APIProvider>
      </Auth0Provider>
  </React.StrictMode>,
)
