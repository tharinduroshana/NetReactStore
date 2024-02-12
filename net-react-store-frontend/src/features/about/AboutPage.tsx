import {
  Alert,
  AlertTitle,
  Button,
  ButtonGroup,
  Container,
  List,
  ListItem,
  ListItemText,
  Typography,
} from "@mui/material";
import agent from "../../app/api/agent";
import { useState } from "react";

const AboutPage = () => {
  const [validationErrors, setValidationErrors] = useState<string[]>([]);

  const getValidationError = () => {
    agent.TestError.getValidationError()
      .then((e) => console.log("Should not see this"))
      .catch((error) => setValidationErrors(error));
  };

  return (
    <Container>
      <Typography gutterBottom variant="h2">
        Error Testing
      </Typography>
      <ButtonGroup fullWidth>
        <Button
          variant="contained"
          onClick={() => agent.TestError.get400Error()}
        >
          Test 400
        </Button>
        <Button
          variant="contained"
          onClick={() =>
            agent.TestError.get401Error().catch((e) => console.log(e))
          }
        >
          Test 401
        </Button>
        <Button
          variant="contained"
          onClick={() => agent.TestError.get404Error()}
        >
          Test 404
        </Button>
        <Button
          variant="contained"
          onClick={() => agent.TestError.get500Error()}
        >
          Test 500
        </Button>
        <Button variant="contained" onClick={getValidationError}>
          Test Validation Error
        </Button>
      </ButtonGroup>
      {validationErrors.length > 0 && (
        <Alert severity="error">
          <AlertTitle>Validation Error</AlertTitle>
          <List>
            {validationErrors.map((error) => (
              <ListItem key={error}>
                <ListItemText>{error}</ListItemText>
              </ListItem>
            ))}
          </List>
        </Alert>
      )}
    </Container>
  );
};

export default AboutPage;
