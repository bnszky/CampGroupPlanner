import moment from "moment";

function humanizeDate(date){
    const now = moment();
    const publishedDate = moment(date);
    const diff = now.diff(publishedDate);

    return `Written ${moment.duration(diff).humanize()} ago`
}

export default humanizeDate;