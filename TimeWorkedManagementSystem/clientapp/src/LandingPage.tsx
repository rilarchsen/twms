import {useAuth0} from "@auth0/auth0-react";
export function LandingPage() {
    const {loginWithRedirect} = useAuth0();
    return (
        <div className={"absolute left-0 top-0 w-full h-full bg-purple-950"}>
            <div className={"items-center justify-center mt-48"}>
                <h1 className={"text-[14rem] text-white font-funky select-none"}>TWMS</h1>
                <div className={"flex my-12 justify-center"}>
                    <button onClick={() => loginWithRedirect()}>
                        Log in
                    </button>
                </div>
            </div>
        </div>
    );
}