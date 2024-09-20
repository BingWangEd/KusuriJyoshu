import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { useCallback, useState } from 'react';
import PromptEditor from './PromptEditor';
import { usePrescriptionContext } from './usePrescriptionContext';

interface IPromptCard {
	id: number;
	date: string;
	content: string;
	patientId: number;
}

const PromptCard = ({ id, date, content, patientId }: IPromptCard) => {
	console.log("content: ", content);
	const [editing, setEditing] = useState(false);
	const { refetch } = usePrescriptionContext();

	const deletePrompt = useCallback(async () => {
		try {
			const response = await fetch(`/api/PrescriptionPrompt/delete/${id}`, {
				method: "POST",
			});

			if (!response.ok) {
				throw new Error("Network response was not ok");
			}

			console.log(response);
			await refetch();
		} catch (error) {
			console.error(error);
		}
	}, [refetch]);

	const savePrompt = useCallback(async (content: string) => {
		try {
			const response = await fetch(`/api/PrescriptionPrompt/edit/${id}`, {
				method: "POST",
				headers: { "Content-Type": "application/json" },
            	body: JSON.stringify(content)
			});

			if (!response.ok) {
				throw new Error("Network response was not ok");
			}

			await refetch();
			console.log(response);
		} catch (error) {
			console.error(error);
		}
	}, [refetch]);

	return editing ? (
		<PromptEditor
			label="処方編集"
			content={content}
			cancel={() => setEditing(false)}
			save={savePrompt}
		/>
    ) : (
		<Card sx={{ marginBottom: "24px" }}>
			<CardContent>
				<Typography gutterBottom variant="body1" component="div">
					{date}
				</Typography>
				<Typography variant="body2" sx={{ color: 'text.secondary' }}>
					{content}
				</Typography>
			</CardContent>
			<CardActions>
				<Button size="small"><EditIcon onClick={() => setEditing(true)}/></Button>
				<Button size="small"><DeleteIcon onClick={deletePrompt} /></Button>
			</CardActions>
		</Card>
	);
}

export default PromptCard;
