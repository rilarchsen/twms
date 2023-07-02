import React, { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import {useAuth0} from "@auth0/auth0-react";
import {LandingPage} from "./LandingPage.tsx";

function App() {
  const [count, setCount] = useState(0)
    const {isLoading, isAuthenticated, error, user, loginWithRedirect, logout, getAccessTokenSilently} = useAuth0();
  const [bearer, setBearer] = useState<string | undefined>(undefined);

  if (isLoading) {
      return <div>Loading ...</div>;
  }

  if (error) {
        return (<>
            <div>Oops... {error.message}</div>
            <button onClick={() => logout({returnTo: window.location.origin})}>Log out</button>
        </>);
  }

  if (!isAuthenticated) {
      return <LandingPage />
  } else {
      getAccessTokenSilently().then((token) => {setBearer(token)}).catch((err) => {console.log(err)});
  }

  return (
    <>
      <div>
        <a href="https://vitejs.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Hi, {user?.name}</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <button onClick={() => logout({returnTo: window.location.origin})}>
          Log out
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
