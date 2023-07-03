import { useAuth0 } from '@auth0/auth0-react';
import { createContext, useContext, useMemo } from "react"
import { ApiClient } from "../ApiClient";
import {AuthorizeService} from "../AuthorizeService.ts";

type APIInterface = {
    apiClient: ApiClient;
}

export const APIContext = createContext<APIInterface | null>(null);
export const useAPI = () => useContext(APIContext) as APIInterface;

type APIProviderProps = {
    children: React.ReactElement;
}

export function APIProvider({ children }: APIProviderProps) {
    const {
        getAccessTokenSilently
    } = useAuth0();

    const authService = new AuthorizeService({getAccessTokenSilently: getAccessTokenSilently});

    const apiClient = new ApiClient(authService);

    const contextValue = useMemo(() => {
        return {
            apiClient,
        };
    }, [
        apiClient,
    ]);

    return (
        <APIContext.Provider value={contextValue}>
            {children}
        </APIContext.Provider>
    )
}