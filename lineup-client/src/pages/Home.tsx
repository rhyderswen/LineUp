import { useAuth0 } from "@auth0/auth0-react/";
import LoggedInHome from "./LoggedInHome";
import LoggedOutHome from "./LoggedOutHome";
import ProtectedApiTest from "../components/auth/ProtectedApiTest.tsx";

const Home = () => {
  const { isAuthenticated, isLoading, error } = useAuth0();

  return (
    <>
      {(!isLoading || error) && (
        <>{isAuthenticated ? <LoggedInHome /> : <LoggedOutHome />}</>
      )}
      <ProtectedApiTest />
    </>
  );
};

export default Home;
