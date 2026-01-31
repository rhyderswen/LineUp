import { useEffect } from "react";
import { Link } from "react-router";

interface Props {
  children: React.ReactNode;
}

const Home = ({ children }: Props) => {
  useEffect(() => {
    console.log(`Home mounted`);
  }, []);

  return (
    <div className="root">
      <div className="topbar">
        <div className="lineUpLogo">
          <Link to="/">Line Up</Link>
        </div>
        <div className="signOutButton">
          <a href="#">Sign Out</a>
        </div>
      </div>
      <hr
        style={{
          borderColor: "var(--primary)",
          borderWidth: "1px",
          borderStyle: "solid",
        }}
      />
      {children}
    </div>
  );
};

export default Home;
