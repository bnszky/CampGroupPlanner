import moment from "moment";

function HumanDate({date}) {

    const now = moment();
    const publishedDate = moment(date);
    const diff = now.diff(publishedDate);

    return <p className="date">Written {moment.duration(diff).humanize()} ago</p>;
}

export default HumanDate;