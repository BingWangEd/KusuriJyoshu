import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import SaveIcon from '@mui/icons-material/Save';
import { useState } from 'react';
import { TextField } from '@mui/material';
import ClearIcon from '@mui/icons-material/Clear';
import { usePrescriptionContext } from './usePrescriptionContext';
import { LoadingButton } from '@mui/lab';

interface IPromptCard {
	content: string;
    label: string;
    save: (content: string) => Promise<void>;
    cancel: () => void;
}

const PromptEditor = ({ label, content, save, cancel }: IPromptCard) => {
    const [prompt, setPrompt] = useState(content);
    const { refetch } = usePrescriptionContext();
    const [sending, setSending] = useState(false);

	return (
		<Card sx={{ marginBottom: "24px", overflow: "scroll", minHeight: "230px" }}>
			<CardContent>
				<Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    <TextField
                        id="outlined-multiline-static"
                        label={<>{label}</>}
                        multiline
                        rows={5}
                        value={prompt}
                        onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                            setPrompt(event.target.value);
                        }}
                        style={{ width: "100%" }}
                    />
				</Typography>
			</CardContent>
			<CardActions>
                {sending ? (
                    <LoadingButton loading loadingPosition="start" startIcon={<SaveIcon />}>
                        送信中...
                    </LoadingButton>
                ) : (
                    <Button size="small" onClick={async () => {
                        setSending(true);
                        await save(prompt);
                        refetch();
                        cancel();
                    }}><SaveIcon /></Button>
                )}

                <Button size="small" onClick={cancel}><ClearIcon /></Button>
			</CardActions>
		</Card>
	);
}

export default PromptEditor;
