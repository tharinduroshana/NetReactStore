import "../../App.css";
import Header from "./Header";
import {
  Container,
  createTheme,
  CssBaseline,
  ThemeProvider,
} from "@mui/material";
import { useState } from "react";
import { Outlet } from "react-router-dom";

const App = () => {
  const [darkMode, setDarkMode] = useState(false);
  const paletteType = darkMode ? "dark" : "light";

  const theme = createTheme({
    palette: {
      mode: paletteType,
      background: { default: paletteType === "light" ? "#eaeaea" : "#121212" },
    },
  });

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Header
        darkMode={darkMode}
        handleThemeChange={() => setDarkMode(!darkMode)}
      />
      <Container>
        <Outlet />
      </Container>
    </ThemeProvider>
  );
};

export default App;
