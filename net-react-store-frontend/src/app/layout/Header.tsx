import { AppBar, Switch, Toolbar, Typography } from "@mui/material";

type HeaderProps = {
  darkMode: boolean;
  handleThemeChange: () => void;
};
const Header = ({ darkMode, handleThemeChange }: HeaderProps) => {
  return (
    <AppBar position="static" sx={{ mb: 4 }}>
      <Toolbar>
        <Typography variant="h6">Net React Store</Typography>
        <Switch checked={darkMode} onChange={handleThemeChange} />
      </Toolbar>
    </AppBar>
  );
};

export default Header;
