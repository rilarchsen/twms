import { useState, Suspense, lazy, } from 'react'
import {useAuth0} from "@auth0/auth0-react";
import {LandingPage} from "./LandingPage.tsx";
import { Route, Routes } from 'react-router-dom';

import Dashboard from './pages/Dashboard.tsx';
import SignIn from './pages/Authentication/SignIn';
import SignUp from './pages/Authentication/SignUp';
import Loader from './common/Loader';
import DefaultLayout from "./layout/DefaultLayout.tsx";
import Calendar from "./pages/Calendar.tsx";
import Profile from "./pages/Profile.tsx";
import FormLayout from "./pages/Form/FormLayout.tsx";
import Tables from "./pages/Tables.tsx";
import Settings from "./pages/Settings.tsx";
import Chart from "./pages/Chart.tsx";
import Buttons from "./pages/UiElements/Buttons.tsx";
import Alerts from "./pages/UiElements/Alerts.tsx";
import FormElements from "./pages/Form/FormElements.tsx";

function App() {
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
  }

  return (
    <>
        <Routes>
            <Route path="/auth/signin" element={<SignIn />} />
            <Route path="/auth/signup" element={<SignUp />} />
            <Route element={<DefaultLayout />}>
                <Route index element={<Dashboard />} />
                <Route
                    path="/calendar"
                    element={
                        <Suspense fallback={<Loader />}>
                            <Calendar />
                        </Suspense>
                    }
                />
                <Route
                    path="/profile"
                    element={
                        <Suspense fallback={<Loader />}>
                            <Profile />
                        </Suspense>
                    }
                />
                <Route
                    path="/forms/form-elements"
                    element={
                        <Suspense fallback={<Loader />}>
                            <FormElements />
                        </Suspense>
                    }
                />
                <Route
                    path="/forms/form-layout"
                    element={
                        <Suspense fallback={<Loader />}>
                            <FormLayout />
                        </Suspense>
                    }
                />
                <Route
                    path="/tables"
                    element={
                        <Suspense fallback={<Loader />}>
                            <Tables />
                        </Suspense>
                    }
                />
                <Route
                    path="/settings"
                    element={
                        <Suspense fallback={<Loader />}>
                            <Settings />
                        </Suspense>
                    }
                />
                <Route
                    path="/chart"
                    element={
                        <Suspense fallback={<Loader />}>
                            <Chart />
                        </Suspense>
                    }
                />
                <Route
                    path="/ui/alerts"
                    element={
                        <Suspense fallback={<Loader />}>
                            <Alerts />
                        </Suspense>
                    }
                />
                <Route
                    path="/ui/buttons"
                    element={
                        <Suspense fallback={<Loader />}>
                            <Buttons />
                        </Suspense>
                    }
                />
            </Route>
        </Routes>
    </>
  )
}

export default App
