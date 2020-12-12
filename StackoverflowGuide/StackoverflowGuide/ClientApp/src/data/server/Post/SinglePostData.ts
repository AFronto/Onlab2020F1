import AnswerData from "../Answer/AnswerData";

export default interface SinglePostData {
  id: string;
  title: string;
  body: string;
  answers: AnswerData[];
  acceptedAnswerId: string;
  tags: string[];
  creationDate: string;
  score: number;
  favoriteCount: number;
  viewCount: number;
}
