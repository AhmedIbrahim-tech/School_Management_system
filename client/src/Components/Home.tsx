import { Card, Row, Col, Container } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { DashboardCardProps } from '../Interface/DashboardCard';

const DashboardCard = ({ title, count, icon, link, color }: DashboardCardProps) => (
  <Card as={Link} to={link} className="mb-4 text-decoration-none">
    <Card.Body>
      <Row>
        <Col xs={8}>
          <div className="d-flex flex-column">
            <h3 className="mb-1">{count}</h3>
            <div className="text-muted">{title}</div>
          </div>
        </Col>
        <Col xs={4} className="text-end">
          <div style={{ color: color, fontSize: '2rem' }}>
            <i className={icon}></i>
          </div>
        </Col>
      </Row>
    </Card.Body>
  </Card>
);

export const Home = () => {
  const dashboardItems: DashboardCardProps[] = [
    {
      title: 'Students',
      count: 150,
      icon: 'bi bi-mortarboard-fill',
      link: '/student',
      color: '#007bff'
    },
    {
      title: 'Teachers',
      count: 45,
      icon: 'bi bi-person-workspace',
      link: '/teacher',
      color: '#28a745'
    },
    {
      title: 'Courses',
      count: 32,
      icon: 'bi bi-book-fill',
      link: '/course',
      color: '#dc3545'
    },
    {
      title: 'Events',
      count: 12,
      icon: 'bi bi-calendar-event-fill',
      link: '/event',
      color: '#ffc107'
    }
  ];

  return (
    <Container className="py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Dashboard</h1>
        <div>
          <Link to="/student" className="btn btn-primary">
            <i className="bi bi-plus-circle me-2"></i>
            Student Management
          </Link>
        </div>
      </div>

      <Row>
        {dashboardItems.map((item, index) => (
          <Col key={index} md={6} lg={3}>
            <DashboardCard {...item} />
          </Col>
        ))}
      </Row>

      <Row className="mt-4">
        <Col md={8}>
          <Card className="mb-4">
            <Card.Header className="bg-white">
              <h5 className="mb-0">Recent Activities</h5>
            </Card.Header>
            <Card.Body>
              <div className="list-group list-group-flush">
                <div className="list-group-item">
                  <i className="bi bi-person-plus text-success me-2"></i>
                  New student registration
                  <small className="text-muted ms-2">5 minutes ago</small>
                </div>
                <div className="list-group-item">
                  <i className="bi bi-book text-primary me-2"></i>
                  Course schedule updated
                  <small className="text-muted ms-2">2 hours ago</small>
                </div>
                <div className="list-group-item">
                  <i className="bi bi-calendar text-info me-2"></i>
                  New event created
                  <small className="text-muted ms-2">1 day ago</small>
                </div>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col md={4}>
          <Card>
            <Card.Header className="bg-white">
              <h5 className="mb-0">Quick Actions</h5>
            </Card.Header>
            <Card.Body>
              <div className="d-grid gap-2">
                <Link to="/student" className="btn btn-outline-primary">
                  <i className="bi bi-person-plus me-2"></i>
                  Student Management
                </Link>
                <Link to="/course" className="btn btn-outline-success">
                  <i className="bi bi-book-half me-2"></i>
                  Course Management
                </Link>
                <Link to="/event" className="btn btn-outline-info">
                  <i className="bi bi-calendar-plus me-2"></i>
                  Schedule Event
                </Link>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};