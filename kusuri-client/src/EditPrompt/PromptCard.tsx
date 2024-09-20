import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { useCallback } from 'react';
interface IPromptCard
{
    date: string;
    content: string;
}

const PromptCard = ({ date, content }: IPromptCard) => {
  
  return (
    <Card sx={{ maxWidth: 345 }}>
      <CardContent>
        <Typography gutterBottom variant="h6" component="div">
          {date}
        </Typography>
        <Typography variant="body2" sx={{ color: 'text.secondary' }}>
          {content}
        </Typography>
      </CardContent>
      <CardActions>
        <Button size="small"><EditIcon /></Button>
        <Button size="small"><DeleteIcon /></Button>
      </CardActions>
    </Card>
  );
}

export default PromptCard;
